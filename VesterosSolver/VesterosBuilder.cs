﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VesterosSolver
{
    public static class VesterosBuilder
    {
        public static Game VesterosMap()
        {
            Game game = new Game();

            #region Строим карту Вестероса
            List<Place> places = game.places;
            {
                float width = 1207;
                float heoght = 800;

                #region Раставляем точки
                places.Add(new Place
                {
                    position = new System.Numerics.Vector2(621, 122),
                    name = "PYK"
                });

                places.Add(new Place
                {
                    position = new System.Numerics.Vector2(572, 56),
                    name = "IRONMAN'S BAY"
                });

                places.Add(new Place
                {
                    position = new System.Numerics.Vector2(450, 46),
                    name = "THE GOLDEN SOUND"
                });

                places.Add(new Place
                {
                    position = new System.Numerics.Vector2(583, 12),
                    name = "SUNSET SEA"
                });

                places.Add(new Place
                {
                    position = new System.Numerics.Vector2(486, 150),
                    name = "LANNISPORT"
                });

                places.Add(new Place
                {
                    position = new System.Numerics.Vector2(552, 248),
                    name = "RIV"
                });

                places.Add(new Place
                {
                    position = new System.Numerics.Vector2(639, 239),
                    name = "SEAGUARD"
                });

                places.Add(new Place
                {
                    position = new System.Numerics.Vector2(723, 201),
                    name = "GREY"
                });
                #endregion

                #region Создаем пути между землями
                places.AddLink("PYK", "IRONMAN'S BAY");
                places.AddLink("THE GOLDEN SOUND", "IRONMAN'S BAY");

                places.AddLink("SUNSET SEA", "IRONMAN'S BAY");
                places.AddLink("THE GOLDEN SOUND", "SUNSET SEA");

                places.AddLink("THE GOLDEN SOUND", "LANNISPORT");

                places.AddLink("THE GOLDEN SOUND", "RIV");
                places.AddLink("RIV", "LANNISPORT");

                places.AddLink("RIV", "SEAGUARD");
                places.AddLink("SEAGUARD", "IRONMAN'S BAY");

                places.AddLink("SEAGUARD", "GREY");
                places.AddLink("GREY", "IRONMAN'S BAY");
                #endregion


                for (int i = 0; i < places.Count; i++)
                {
                    places[i].position.X /= width;
                    places[i].position.Y /= heoght;
                }
            }
            #endregion

            return game;

        }
    }
}
