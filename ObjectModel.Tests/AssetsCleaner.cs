using System;
using System.Collections.Generic;

namespace VersionOne.SDK.ObjectModel.Tests {
    internal class AssetsCleaner {
        private readonly Stack<BaseAsset> baseAssets;
        private readonly Stack<Entity> entities;
        private readonly Project defaultProject;

        internal AssetsCleaner(Stack<BaseAsset> baseAssets, Stack<Entity> entities, Project defaultProject) {
            this.baseAssets = baseAssets;
            this.entities = entities;
            this.defaultProject = defaultProject;
        }

        internal void Delete() {
            ProcessEntities();
            DeleteBaseAssets();            
        }

        private void ProcessEntities() {
            while (entities.Count > 0) {
                var item = entities.Pop();

                if (item is Environment) {
                    AssignEnvironmentToDefaultScope((Environment)item);
                }
            }
        }

        private void AssignEnvironmentToDefaultScope(Environment environment) {
            if (environment.IsClosed) {
                environment.Reactivate();
            }

            environment.Project = defaultProject;
            environment.Save();
        }

        private void DeleteBaseAssets() {
            while (baseAssets.Count > 0) {
                var item = baseAssets.Pop();
                PrepareAssetForDelete(item);
                DeleteAsset(item);
            }
        }

        private static void PrepareAssetForDelete(BaseAsset item) {
            try {
                ReopenProjectIfClosed(item);
                ReopenIfClosed(item);
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ReopenProjectIfClosed(BaseAsset item) {
            if (!(item is Workitem)) {
                return;
            }

            var workitem = ((Workitem)item);
            
            if (workitem.Project.IsClosed && workitem.Project.CanReactivate) {
                ReactivateAsset(workitem.Project);
            }
        }

        private static void ReopenIfClosed(BaseAsset item) {
            if (item.IsClosed && item.CanReactivate) {
                ReactivateAsset(item);
            }
        }

        private static void ReactivateAsset(BaseAsset item) {
            try {
                item.Reactivate();
            } catch (Exception ex) {
                Console.WriteLine(string.Format("Can't reactivate {0} item.", item.ID.Token));
                Console.WriteLine(ex.Message);
            }
        }

        private static void DeleteAsset(BaseAsset item) {
            try {
                item.Delete();
            } catch (Exception ex) {
                Console.WriteLine(string.Format("Can't delete {0} item.", item.ID.Token));
                Console.WriteLine(ex.Message);
            }
        } 
    }
}