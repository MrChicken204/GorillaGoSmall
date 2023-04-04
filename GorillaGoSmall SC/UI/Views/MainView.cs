/*
    I am not putting in a UISelectionHandler as it is not needed due to there only being one option. 
*/


using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Text;

namespace GorillaGoSmallGorillaGoBig.UI.Views
{
    internal class MainView : ComputerView
    {
        // Executes when the view is requested to be shown
        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            Build();
        }

        /// <summary>
        /// Whenever a key is pressed ( Only if this view is active )
        /// </summary>
        /// <param name="key">Key pressed</param>
        public override void OnKeyPressed(EKeyboardKey key)
        {
            base.OnKeyPressed(key);

            switch (key)
            {
                case EKeyboardKey.Left:
                    Plugin.Size -= 0.1f;
                    Build(); // Rebuild
                    break;
                case EKeyboardKey.Right:
                    Plugin.Size += 0.1f;
                    Build(); // Rebuild
                    break;

                case EKeyboardKey.Back:
                    ReturnToMainMenu();
                    break;


            }
        }


        /// <summary>
        /// This builds the screen using a stringbuilder. Then uses the SetText() to set the text of the screen.
        /// </summary>
        private void Build()
        {
            string Size_Text = $"< {Plugin.Size} >";

            // Text to-be set
            StringBuilder stringBuilder = Header()
                .AppendLine(Size_Text);

            // Sets the text
            SetText(stringBuilder);
        }

        /// <summary>
        /// This is the top of the screen.
        /// </summary>
        /// <returns>First bar, title, author, second bar, 3 lines ( For offset )</returns>
        private StringBuilder Header()
        {
            return
                new StringBuilder()
                .BeginCenter()
                .MakeBar('-', SCREEN_WIDTH / 2, 0)
                .AppendLine($"\n{PluginInfo.Name}")
                .AppendLine("<color=#696969>By MrBanana The Pie with some help by Crafter Bot</color>")
                .MakeBar('-', SCREEN_WIDTH / 2, 0)
                .AppendLines(5);
        }
    }
}
