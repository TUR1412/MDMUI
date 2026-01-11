using System;
using System.Collections.Generic;
using MDMUI.DAL;
using MDMUI.Model;

namespace MDMUI.BLL
{
    public class SystemParameterService
    {
        private readonly SystemParameterDAL parameterDal = new SystemParameterDAL();
        private readonly object cacheGate = new object();
        private Dictionary<string, SystemParameter> cache;
        private DateTime cacheLoadedUtc = DateTime.MinValue;
        private readonly TimeSpan cacheTtl = TimeSpan.FromMinutes(2);

        public IReadOnlyList<SystemParameter> GetAllParameters()
        {
            return parameterDal.GetAll();
        }

        public string GetString(string key, string defaultValue = null)
        {
            SystemParameter parameter = GetParameter(key);
            if (parameter == null || string.IsNullOrWhiteSpace(parameter.ParamValue))
            {
                return defaultValue;
            }

            return parameter.ParamValue.Trim();
        }

        public int GetInt(string key, int defaultValue)
        {
            string value = GetString(key, null);
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;

            if (int.TryParse(value, out int result))
            {
                return result;
            }

            return defaultValue;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            string value = GetString(key, null);
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;

            if (bool.TryParse(value, out bool boolResult))
            {
                return boolResult;
            }

            if (string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)) return true;
            if (string.Equals(value, "0", StringComparison.OrdinalIgnoreCase)) return false;

            return defaultValue;
        }

        public TimeSpan GetTimeSpanMinutes(string key, int defaultMinutes)
        {
            int minutes = GetInt(key, defaultMinutes);
            if (minutes <= 0) minutes = defaultMinutes;
            return TimeSpan.FromMinutes(minutes);
        }

        public void SetValue(string key, string value, string description = null)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            parameterDal.Upsert(key.Trim(), value?.Trim(), description);
            RefreshCache();
        }

        public void RefreshCache()
        {
            lock (cacheGate)
            {
                cache = null;
                cacheLoadedUtc = DateTime.MinValue;
            }
        }

        private SystemParameter GetParameter(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;
            EnsureCache();

            lock (cacheGate)
            {
                if (cache != null && cache.TryGetValue(key, out SystemParameter parameter))
                {
                    return parameter;
                }
            }

            return null;
        }

        private void EnsureCache()
        {
            lock (cacheGate)
            {
                if (cache != null && (DateTime.UtcNow - cacheLoadedUtc) < cacheTtl)
                {
                    return;
                }

                cache = new Dictionary<string, SystemParameter>(StringComparer.OrdinalIgnoreCase);
                foreach (SystemParameter parameter in parameterDal.GetAll())
                {
                    if (parameter == null || string.IsNullOrWhiteSpace(parameter.ParamKey)) continue;
                    cache[parameter.ParamKey] = parameter;
                }

                cacheLoadedUtc = DateTime.UtcNow;
            }
        }
    }
}
