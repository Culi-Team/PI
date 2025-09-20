using EQX.Core.Units;
using Microsoft.Extensions.Configuration;
using PIFilmAutoDetachCleanMC.Recipe;
using System;
using System.Collections.Generic;
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

        public void RecipeUpdateHandle()
        {
            CassetteIn.Rows = _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.CasetteRows;
            CassetteIn.Columns = 1;
            CassetteIn.GenerateCells();

            CassetteOut.Rows = _recipeSelector.CurrentRecipe.CstLoadUnloadRecipe.CasetteRows;
            CassetteOut.Columns = 1;
            CassetteOut.GenerateCells();
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
    }
}
