using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Godot;

namespace RogueCustomsGodotClient.Utils
{
    public static class GlobalConstants
    {
        public static readonly string GameVersion = Assembly.GetExecutingAssembly()
                                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                        .InformationalVersion;

        public static readonly StyleBoxFlat PopUpBorderStyle = GD.Load<StyleBoxFlat>("res://Styles/PopUpBorder.tres");
        public static readonly StyleBoxFlat PopUpTitleStyle = GD.Load<StyleBoxFlat>("res://Styles/PopUpTitle.tres");
        public static readonly StyleBoxFlat ButtonHoverStyle = GD.Load<StyleBoxFlat>("res://Styles/ButtonHover.tres");
        public static readonly StyleBoxFlat ButtonNormalStyle = GD.Load<StyleBoxFlat>("res://Styles/ButtonNormal.tres");

        public static readonly StyleBoxFlat HeaderCellStyleBox = new()
        {
            BgColor = new Color() { R8 = 0, G8 = 125, B8 = 125, A = 1 },
            BorderColor = new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 },
            BorderWidthBottom = 1,
            BorderWidthLeft = 1,
            BorderWidthRight = 1,
            BorderWidthTop = 1,
            ContentMarginBottom = 8,
            ContentMarginTop = 8,
            ContentMarginLeft = 12,
            ContentMarginRight = 12
        };
        public static readonly StyleBoxFlat NormalCellStyleBox = new()
        {
            BgColor = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 },
            BorderColor = new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 },
            BorderWidthBottom = 1,
            BorderWidthLeft = 1,
            BorderWidthRight = 1,
            BorderWidthTop = 1,
            ContentMarginBottom = 8,
            ContentMarginTop = 8,
            ContentMarginLeft = 12,
            ContentMarginRight = 12
        };
        public static readonly StyleBoxFlat SelectedCellStyleBox = new()
        {
            BgColor = new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 },
            BorderColor = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 },
            BorderWidthBottom = 1,
            BorderWidthLeft = 1,
            BorderWidthRight = 1,
            BorderWidthTop = 0,
            ContentMarginBottom = 8,
            ContentMarginTop = 8,
            ContentMarginLeft = 12,
            ContentMarginRight = 12
        };
        public static readonly StyleBoxFlat NormalItemStyleBox = new()
        {
            BgColor = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 },
            BorderWidthBottom = 0,
            BorderWidthLeft = 0,
            BorderWidthRight = 0,
            BorderWidthTop = 0,
            ContentMarginBottom = 0,
            ContentMarginTop = 0,
            ContentMarginLeft = 12,
            ContentMarginRight = 12
        };
        public static readonly StyleBoxFlat SelectedItemStyleBox = new()
        {
            BgColor = new Color() { R8 = 255, G8 = 255, B8 = 255, A = 1 },
            BorderWidthBottom = 0,
            BorderWidthLeft = 0,
            BorderWidthRight = 0,
            BorderWidthTop = 0,
            ContentMarginBottom = 0,
            ContentMarginTop = 0,
            ContentMarginLeft = 12,
            ContentMarginRight = 12
        };
        public static readonly StyleBoxFlat TileStyleBox = new()
        {
            BgColor = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 },
            BorderColor = new Color() { R8 = 0, G8 = 0, B8 = 0, A = 1 },
            BorderWidthBottom = 0,
            BorderWidthLeft = 0,
            BorderWidthRight = 01,
            BorderWidthTop = 0,
            ContentMarginBottom = 0,
            ContentMarginTop = 0,
            ContentMarginLeft = 0,
            ContentMarginRight = 0
        };
    }
}
