using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable CA1416 // Validar la compatibilidad de la plataforma
namespace RogueCustomsDungeonEditor.Utils
{
    public class ColorDialogHandler
    {
        private static ColorDialog _instance;
        private static Queue<int> _customColors;

        public static (DialogResult Result, Color Color) Show(Color? defaultSelection)
        {
            _instance ??= new ColorDialog();
            _customColors ??= [];
            _instance.Color = defaultSelection ?? Color.White;

            try
            {
                _instance.CustomColors = _customColors.ToArray();
            }
            catch
            {
                // Ignore invalid colors
            }

            var result = _instance.ShowDialog();

            if (result == DialogResult.OK)
            {
                var translatedColor = ColorTranslator.ToOle(_instance.Color);
                if (!_customColors.Contains(translatedColor))
                {
                    _customColors.Enqueue(translatedColor);
                    while (_customColors.Count > 16)
                    {
                        _customColors.Dequeue();
                    }
                }
            }

            return (result, _instance.Color);
        }
    }
}
#pragma warning restore CA1416 // Validar la compatibilidad de la plataforma
