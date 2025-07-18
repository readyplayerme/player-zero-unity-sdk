﻿using System;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;

namespace PlayerZero.Editor
{
    public abstract class PackageManagerEventListener
    {
        public static event Action<string> OnPackageImported;

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            Events.registeredPackages += OnPackagesInstalled;
        }
        
        ~PackageManagerEventListener()
        {
            Events.registeredPackages -= OnPackagesInstalled;
        }

        static void OnPackagesInstalled(PackageRegistrationEventArgs packageRegistrationEventArgs)
        {
            packageRegistrationEventArgs.added
                .ToList()
                .ForEach(packageInfo =>
                {
                    OnPackageImported?.Invoke(packageInfo.name);
                });
        }
    }
}
