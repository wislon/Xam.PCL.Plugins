﻿/*
 * MvxSettings:
 * Copyright (C) 2014 Refractored: 
 * 
 * Contributors:
 * http://github.com/JamesMontemagno
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */

using Windows.Storage;
using Refractored.Xam.Settings.Abstractions;
using System;

namespace Refractored.Xam.Settings
{
  /// <summary>
  /// Main ISettings Implementation
  /// </summary>
  public class Settings : ISettings
  {
    private static ApplicationDataContainer AppSettings
    {
      get { return ApplicationData.Current.LocalSettings; }
    }

    private readonly object m_Locker = new object();

    /// <summary>
    /// Gets the current value or the default that you specify.
    /// </summary>
    /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
    /// <param name="key">Key for settings</param>
    /// <param name="defaultValue">default value if not set</param>
    /// <returns>Value or default</returns>
    public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
    {
      object value;
      lock (m_Locker)
      {
        if(typeof(T) == typeof(decimal))
        {
          string savedDecimal;
          // If the key exists, retrieve the value.
          if (AppSettings.Values.ContainsKey(key))
          {
            savedDecimal = (string)AppSettings.Values[key];
          }
          // Otherwise, use the default value.
          else
          {
            savedDecimal = defaultValue.ToString();
          }

          value = Convert.ToDecimal(savedDecimal);

          return null != value ? (T)value : defaultValue;
        }
        else if (typeof(T) == typeof(DateTime))
        {
          string savedTime = null;
          // If the key exists, retrieve the value.
          if (AppSettings.Values.ContainsKey(key))
          {
            savedTime = (string)AppSettings.Values[key];
          }

          var ticks = string.IsNullOrWhiteSpace(savedTime) ? -1 : Convert.ToInt64(savedTime);
          if (ticks == -1)
            value = defaultValue;
          else
            value = new DateTime(ticks);

          return null != value ? (T)value : defaultValue;
        }
       
        // If the key exists, retrieve the value.
        if (AppSettings.Values.ContainsKey(key))
        {
          value = (T)AppSettings.Values[key];
        }
        // Otherwise, use the default value.
        else
        {
          value = defaultValue;
        }
      }

      return null != value ? (T)value : defaultValue;
    }

    /// <summary>
    /// Adds or updates the value 
    /// </summary>
    /// <param name="key">Key for settting</param>
    /// <param name="value">Value to set</param>
    /// <returns>True of was added or updated and you need to save it.</returns>
    public bool AddOrUpdateValue(string key, object value)
    {
      bool valueChanged = false;
      lock (m_Locker)
      {

        if (value is decimal)
        {
          return AddOrUpdateValue(key, Convert.ToString(value));
        }
        else if (value is DateTime)
        {
          return AddOrUpdateValue(key, Convert.ToString(((DateTime)value).Ticks));
        }


        // If the key exists
        if (AppSettings.Values.ContainsKey(key))
        {

          // If the value has changed
          if (AppSettings.Values[key] != value)
          {
            // Store key new value
            AppSettings.Values[key] = value;
            valueChanged = true;
          }
        }
        // Otherwise create the key.
        else
        {
          AppSettings.CreateContainer(key, ApplicationDataCreateDisposition.Always);
          AppSettings.Values[key] = value;
          valueChanged = true;
        }
      }

      return valueChanged;
    }

    /// <summary>
    /// Saves any changes out.
    /// </summary>
    [Obsolete("Save is deprecated and settings are automatically saved when AddOrUpdateValue is called.")]
    public void Save()
    {
      //nothing to do it is automatic
    }

  }
}
