using Gacha_Game_2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gacha_Game_2.GameData {
    public static class DisplayFormatting {
        private const int CardImageBorderThickness = 7;
        private const int CardImageBorderCornerRadius = 10;
        
        /// <summary>
        /// Creates the GUI Component of a CardImage
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Border CardImage(Card c) {
            if (c == null) return new Border();

            return new Border {
                Child = new Image {
                    Height = 198,
                    Width = 145,
                    Source = new BitmapImage(new Uri(c.ImgURL))
                },
                BorderThickness = new Thickness(CardImageBorderThickness),
                BorderBrush = Globals.EDBorderColors[c.Edition - 1],
                Height = 198,
                CornerRadius = new CornerRadius(CardImageBorderCornerRadius),
                Width = 145,
            };
        }
        public static Border CardImage(Card c, int height, int width) {
            return new Border {
                Child = new Image {
                    Height = height,
                    Width = width,
                    Source = new BitmapImage(new Uri(c.ImgURL))
                },
                BorderThickness = new Thickness(CardImageBorderThickness),
                BorderBrush = Globals.EDBorderColors[c.Edition - 1],
                Height = height,
                CornerRadius = new CornerRadius(CardImageBorderCornerRadius),
                Width = width,
            };
        }

        public static Button WorkWindowWorkerPosBTN(Card c, string content, RoutedEventHandler clickEvent) {
            Button b = new Button {
                Content = content,
                Tag = c,
                Margin = new Thickness(5),
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128)),
                BorderThickness = new Thickness(2),
            };
            b.Click += clickEvent;
            return b;
        }

        public static TextBlock WorkWindowWorkerInfo(Card c, int pos) {
            if (c == null) {
                return new TextBlock();
            }

            return new TextBlock {
                Text = $"{c.Name}\n" +
                $"{c.Anime}\n" +
                $"Ef: {c.Effort}\n" +
                $"{new ReadOnlySpan<char>("ABCD".ToCharArray()).Slice(pos, 1).ToString()}", // hehehe overkill
            };
        }



    }
}
