﻿using System;
using System.Collections.Generic;
using System.Linq;
using Accelerider.Windows.Infrastructure.FileTransferService.Impls;

namespace Accelerider.Windows.Infrastructure.FileTransferService
{
    public class DefaultDownloaderBuilder : ITransporterBuilder<IDownloader>
    {
        private List<string> _fromPaths = new List<string>();
        private TransporterSettings _settings = new TransporterSettings();

        private string _toPath;

        public ITransporterBuilder<IDownloader> From(string path)
        {
            _fromPaths.Add(path);
            return this;
        }

        public ITransporterBuilder<IDownloader> From(IEnumerable<string> paths)
        {
            _fromPaths.AddRange(paths);
            return this;
        }

        public ITransporterBuilder<IDownloader> To(string path)
        {
            _toPath = path;
            return this;
        }

        public ITransporterBuilder<IDownloader> To(IEnumerable<string> paths)
        {
            _toPath = paths.LastOrDefault();
            return this;
        }

        public ITransporterBuilder<IDownloader> Configure(Action<TransporterSettings> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            settings(_settings);
            return this;
        }

        public ITransporterBuilder<IDownloader> Clone()
        {
            var result = new DefaultDownloaderBuilder();

            var fromPathsCopy = new string[_fromPaths.Count];
            _fromPaths.CopyTo(fromPathsCopy);
            result._fromPaths = new List<string>(fromPathsCopy);

            var settingsCopy = new TransporterSettings();
            _settings.CopyTo(settingsCopy);
            result._settings = settingsCopy;

            result._toPath = _toPath;

            return result;
        }

        public IDownloader Build()
        {
            var result = new DownloaderImpl();
            result.Update(_fromPaths.Select(item => new Uri(item)), _toPath, _settings);
            return result;
        }
    }
}
