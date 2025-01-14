﻿using PnP.Core.QueryModel;
using PnP.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PnP.Core.Model.SharePoint
{
    internal sealed class FeatureCollection : QueryableDataModelCollection<IFeature>, IFeatureCollection
    {
        public FeatureCollection(PnPContext context, IDataModelParent parent, string memberName = null)
           : base(context, parent, memberName)
        {
            PnPContext = context;
            Parent = parent;
        }

        public async Task DisableAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!items.Any(o => o.DefinitionId == id))
            {
                throw new ArgumentOutOfRangeException(nameof(id),
                    PnPCoreResources.Exception_Feature_CannotDeactivateNotActive);
            }

            var feature = items.FirstOrDefault(o => o.DefinitionId == id);

            if (feature != default)
            {
                var ftr = feature as Feature;
                await ftr.RemoveAsync().ConfigureAwait(false);
                items.Remove(feature);
            }
        }

        public void Disable(Guid id)
        {
            DisableAsync(id).GetAwaiter().GetResult();
        }

        public async Task DisableBatchAsync(Guid id)
        {
            await DisableBatchAsync(PnPContext.CurrentBatch, id).ConfigureAwait(false);
        }

        public void DisableBatch(Guid id)
        {
            DisableBatchAsync(id).GetAwaiter().GetResult();
        }

        public async Task DisableBatchAsync(Batch batch, Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!items.Any(o => o.DefinitionId == id))
            {
                throw new ArgumentOutOfRangeException(nameof(id),
                    PnPCoreResources.Exception_Feature_CannotDeactivateNotActive);
            }

            var feature = items.FirstOrDefault(o => o.DefinitionId == id);

            if (feature != default)
            {
                var ftr = feature as Feature;
                await ftr.RemoveBatchAsync(batch).ConfigureAwait(false);
                items.Remove(feature);
            }
        }

        public void DisableBatch(Batch batch, Guid id)
        {
            DisableBatchAsync(batch, id).GetAwaiter().GetResult();
        }

        public async Task<IFeature> EnableAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (items.Any(o => o.DefinitionId == id))
            {
                throw new ArgumentOutOfRangeException(nameof(id),
                    PnPCoreResources.Exception_Feature_AlreadyActivated);
            }

            var feature = CreateNewAndAdd() as Feature;

            feature.DefinitionId = id;

            return await feature.AddAsync().ConfigureAwait(false) as Feature;
        }

        public IFeature Enable(Guid id)
        {
            return EnableAsync(id).GetAwaiter().GetResult();
        }

        public async Task<IFeature> EnableBatchAsync(Guid id)
        {
            return await EnableBatchAsync(PnPContext.CurrentBatch, id).ConfigureAwait(false);
        }

        public IFeature EnableBatch(Guid id)
        {
            return EnableBatchAsync(id).GetAwaiter().GetResult();
        }

        public async Task<IFeature> EnableBatchAsync(Batch batch, Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (items.Any(o => o.DefinitionId == id))
            {
                throw new ArgumentOutOfRangeException(nameof(id),
                    PnPCoreResources.Exception_Feature_AlreadyActivated);
            }

            var feature = CreateNewAndAdd() as Feature;
            feature.DefinitionId = id;

            return await feature.AddBatchAsync(batch).ConfigureAwait(false) as Feature;
        }

        public IFeature EnableBatch(Batch batch, Guid id)
        {
            return EnableBatchAsync(batch, id).GetAwaiter().GetResult();
        }

    }
}
