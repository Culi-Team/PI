using EQX.Core.Units;
using EQX.UI.Controls;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PIFilmAutoDetachCleanMC.Converters;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Defines.Devices.Cassette
{
    public class CassetteList
    {
        private readonly RecipeSelector _recipeSelector;
        private readonly IConfiguration _configuration;
        private ITray<ETrayCellStatus> cassetteIn;
        private ITray<ETrayCellStatus> cassetteOut;

        private string BackupFolder => _configuration.GetValue<string>("Folders:BackupFolder") ?? "";

        public ITray<ETrayCellStatus> CassetteIn
		{
			get { return cassetteIn; }
			set { cassetteIn = value; }
		}

        public ITray<ETrayCellStatus> CassetteOut
        {
            get { return cassetteOut; }
            set { cassetteOut = value; }
        }

        public CassetteList(RecipeSelector recipeSelector,
            IConfiguration configuration)
        {
            _recipeSelector = recipeSelector;
            _configuration = configuration;
            CassetteIn = new Tray<ETrayCellStatus>("CassetteIn");
            CassetteOut = new Tray<ETrayCellStatus>("CassetteOut");
        }


        private void ResetCassetteStatus(ITray<ETrayCellStatus> cassette)
        {
            if (cassette.Cells.Any(c => c.Status != ETrayCellStatus.Ready))
            {
                cassette.SetAllCell(ETrayCellStatus.Ready);
            }
            else
            {
                cassette.SetAllCell(ETrayCellStatus.Skip);
            }
        }

        public void ResetCSTOut()
        {
            ResetCassetteStatus(CassetteOut);
        }
        public void ResetCSTIn()
        {
            ResetCassetteStatus(CassetteIn);
        }

        public void RecipeUpdateHandle()
        {
            CassetteIn.Rows = _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.CasetteRows;
            CassetteIn.Columns = 1;
            CassetteIn.GenerateCells();

            CassetteOut.Rows = _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.CasetteRows;
            CassetteOut.Columns = 1;
            CassetteOut.GenerateCells();

            SubscribeCellClickedEvent();
        }

        public void SubscribeCellClickedEvent()
        {
            CassetteIn.Cells?.ToList().ForEach(cell =>
            {
                cell.CellClicked += (id, status) =>
                {
                    if (status == ETrayCellStatus.Skip) cell.Status = ETrayCellStatus.Ready;
                    else cell.Status = ETrayCellStatus.Skip;
                };
            });

            CassetteOut.Cells?.ToList().ForEach(cell =>
            {
                cell.CellClicked += (id, status) =>
                {
                    if (status == ETrayCellStatus.Skip) cell.Status = ETrayCellStatus.Ready;
                    else cell.Status = ETrayCellStatus.Skip;
                };
            });
        }

        public void Save()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            settings.Converters.Add(new CassetteConverter());
            string backupTrayAndCassette = JsonConvert.SerializeObject(this, Formatting.Indented, settings);
            string backupTrayAndCassetteFile = Path.Combine(BackupFolder, "CassetteList.json");

            File.WriteAllText(backupTrayAndCassetteFile, backupTrayAndCassette);
        }

        public bool Load()
        {
            string backupTrayAndCassetteFile = Path.Combine(BackupFolder, "CassetteList.json");

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            settings.Converters.Add(new CassetteConverter());

            if (File.Exists(backupTrayAndCassetteFile) == false)
            {
                File.WriteAllText(backupTrayAndCassetteFile, JsonConvert.SerializeObject(this, settings));
            }
            string backupTrayAndCassetteFileContent = File.ReadAllText(backupTrayAndCassetteFile);
            try
            {
                CassetteList trayCassetteList = JsonConvert.DeserializeObject<CassetteList>(backupTrayAndCassetteFileContent, settings);

                CassetteIn.Rows = _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.CasetteRows;
                CassetteIn.Columns = 1;

                CassetteOut.Rows = _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.CasetteRows;
                CassetteOut.Columns = 1;

                CassetteIn.GenerateCells();
                CassetteOut.GenerateCells();

                foreach (var cell in trayCassetteList.CassetteIn.Cells)
                {
                    CassetteIn.Cells.FirstOrDefault(c => c.Id == cell.Id, new TrayCell<ETrayCellStatus>(0)).Status = cell.Status;
                }
                foreach (var cell in trayCassetteList.CassetteOut.Cells)
                {
                    CassetteOut.Cells.FirstOrDefault(c => c.Id == cell.Id, new TrayCell<ETrayCellStatus>(0)).Status = cell.Status;
                }
                if (CassetteIn.Cells == null) CassetteIn.GenerateCells();
                if (CassetteOut.Cells == null) CassetteOut.GenerateCells();
            }
            catch (Exception ex)
            {
                MessageBoxEx.ShowDialog(ex.Message);
                return false;
            }

            return true;
        }
    }
}
