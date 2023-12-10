using System.Drawing;
using System.Reflection;
using System.Text;

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        List<Icon> _icons = new List<Icon>();
        Random _random = new Random();
        Panel firstSelection, secondSelection;
        Panel firstCoverSelection, secondCoverSelection;
        Dictionary<string, int> assignedPanels = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
            LoadImagesFromFiles();
            PopulateIconsToTable();
            ShowCardsInit(true);
        }

        private void ShowCardsInit(bool showCards)
        {
            pnlCover1.Visible = !showCards;
            pnlCover2.Visible = !showCards;
            pnlCover3.Visible = !showCards;
            pnlCover4.Visible = !showCards;
            pnlCover5.Visible = !showCards;
            pnlCover6.Visible = !showCards;
            pnlCover7.Visible = !showCards;
            pnlCover8.Visible = !showCards;
            pnlCover9.Visible = !showCards;
            pnlCover10.Visible = !showCards;
            pnlCover11.Visible = !showCards;
            pnlCover12.Visible = !showCards;
            pnlCover13.Visible = !showCards;
            pnlCover14.Visible = !showCards;
            pnlCover15.Visible = !showCards;
            pnlCover16.Visible = !showCards;

            timerInit.Start();
        }

        private void timerInit_Tick(object sender, EventArgs e)
        {
            timerInit.Stop();
            //Despues de que el temporizador pare, escondemos las cartas
            ShowCardsInit(false);
            //Para asegurarnos de que el temporizador no vuelva a iniciarse, hacemos un .Dispose para anular el temporizador.
            timerInit.Dispose();
        }

        private void LoadImagesFromFiles()
        {
            var files = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            int id = 1;

            foreach (var picture in files)
            {
                if (!picture.EndsWith(".png"))
                    continue;

                var icon = new Icon
                {
                    Id = id,
                    Name = picture.Replace("MemoryGame.Resources.", "").Replace(".png", ""),
                    Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(picture))
                };

                _icons.Add(icon);
                _icons.Add(icon);

                id++;
            }
        }

        private void PopulateIconsToTable()
        {
            int randomNumber;
            Panel panel;

            foreach (var cell in this.Controls)
            {
                if (cell is Panel && !((Panel)cell).Name.Contains("pnlCover"))
                    panel = (Panel)cell;
                else
                    continue;

                randomNumber = _random.Next(0, _icons.Count);

                panel.BackgroundImage = _icons[randomNumber].Image;

                assignedPanels.Add(panel.Name, _icons[randomNumber].Id);

                _icons.RemoveAt(randomNumber);
            }
        }

        private void pnlCover_Click(object sender, EventArgs e)
        {
            if (firstSelection != null && secondSelection != null)
                return;

            Panel clickedPanel = (Panel)sender;

            if (clickedPanel == null)
                return;
            if (clickedPanel.Visible)
                return;

            clickedPanel.Visible = false;

            if (firstSelection == null)
            {
                firstSelection = GetIconPanel(clickedPanel);
                firstCoverSelection = clickedPanel;
                return;
            }

            if (secondSelection == null)
            {
                secondSelection = GetIconPanel(clickedPanel);
                secondCoverSelection = clickedPanel;
            }

            if (firstSelection != null && secondSelection != null && CheckForMatch())
            {
                CleanSelections(true);
            }
            else
            {
                ResetUnmatched();
            }
        }

        private Panel GetIconPanel(Panel coverPanel)
        {
            Panel iconPanel = null;

            foreach (var item in this.Controls)
            {
                if (item is Panel
                    && ((Panel)item).Name.Contains("pnlCover")
                    && ((Panel)item).Tag == coverPanel.Tag)
                {
                    iconPanel = (Panel)item;
                }
            }

            return iconPanel;
        }

        private bool CheckForMatch()
        {
            return assignedPanels[firstSelection.Name] == assignedPanels[secondSelection.Name];
        }

        private void CleanSelections(bool match)
        {
            if (!match)
            {
                firstCoverSelection.Visible = true;
                secondCoverSelection.Visible = true;

            }

            firstCoverSelection = null;
            secondCoverSelection = null;
            firstSelection = null;
            secondSelection = null;

        }

        private void ResetUnmatched()
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CleanSelections(false);
            timer1.Stop();
        }
    }
}
