﻿using HomeSeer.Jui.Views;
using Hspi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HSPI_ZWaveParametersTest
{
    [TestClass]
    public class SettingsPagesTest
    {
        [TestMethod]
        public void CreateDefault()
        {
            var page = SettingsPages.CreateDefault();

            Assert.IsNotNull(page);

            foreach (var view in page.Views)
            {
                TestHelper.VeryHtmlValid(view.ToHtml());
            }

            TestHelper.VeryHtmlValid(page.ToHtml());

            Assert.IsTrue(page.ContainsViewWithId(SettingsPages.LogToFileId));
            Assert.IsTrue(page.ContainsViewWithId(SettingsPages.LoggingDebugId));
        }

        [DataTestMethod]
        [DataRow(false, false)]
        [DataRow(true, false)]
        [DataRow(false, true)]
        [DataRow(true, true)]
        public void DefaultValues(bool enableDefaultLogging, bool logToFileEnable)
        {
            var settingsCollection = new SettingsCollection
            {
                SettingsPages.CreateDefault(enableDefaultLogging, logToFileEnable)
            };

            var settingPages = new SettingsPages(settingsCollection);

            Assert.AreEqual(settingPages.DebugLoggingEnabled, enableDefaultLogging);
            Assert.AreEqual(settingPages.LogtoFileEnabled, logToFileEnable);
        }

        [TestMethod]
        public void OnSettingChangeWithNoChange()
        {
            var settingsCollection = new SettingsCollection
            {
                SettingsPages.CreateDefault()
            };
            var settingPages = new SettingsPages(settingsCollection);

            Assert.IsFalse(settingPages.OnSettingChange(new ToggleView("id", "name")));
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void OnSettingChangeWithDebugLoggingChange(bool initialValue)
        {
            var settingsCollection = new SettingsCollection
            {
                SettingsPages.CreateDefault(enableDefaultLogging: initialValue)
            };
            var settingPages = new SettingsPages(settingsCollection);

            ToggleView changedView = new(SettingsPages.LoggingDebugId, "name", !initialValue);
            Assert.IsTrue(settingPages.OnSettingChange(changedView));
            Assert.AreEqual(settingPages.DebugLoggingEnabled, !initialValue);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void OnSettingChangeWithLogtoFileChange(bool initialValue)
        {
            var settingsCollection = new SettingsCollection
            {
                SettingsPages.CreateDefault(logToFileEnable: initialValue)
            };
            var settingPages = new SettingsPages(settingsCollection);

            ToggleView changedView = new(SettingsPages.LogToFileId, "name", !initialValue);
            Assert.IsTrue(settingPages.OnSettingChange(changedView));
            Assert.AreEqual(settingPages.LogtoFileEnabled, !initialValue);
        }
    }
}