﻿using Microsoft.AspNetCore.Http;
using System;

namespace ExpoGAN.Web.Services
{
    public static class SessionExtensions
    {
        public static bool? GetBoolean(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return BitConverter.ToBoolean(data, 0);
        }
        public static void SetBoolean(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }
    }
}
