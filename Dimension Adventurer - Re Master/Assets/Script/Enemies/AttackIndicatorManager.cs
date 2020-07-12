using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionAdventurer.Enemies
{
    public class AttackIndicatorManager : MonoBehaviour
    {
        public static AttackIndicatorManager singleton;

        /// <summary>
        /// Data for adjusting the width and transform of each indicator
        /// </summary>
        public EnvironmentData EnvironmentData;
        public Dictionary<WorldPosition, AttackIndicator> indicators;
        public float yOffset = .5f;

        private void Awake()
        {
            if (singleton == null)
                singleton = this;

            indicators = new Dictionary<WorldPosition, AttackIndicator>();
        }

        public AttackIndicator GetIndicator(WorldPosition position)
        {
            AttackIndicator ai;
            if (indicators.TryGetValue(position, out ai))
                return ai;
            else
            {
                Debug.LogError("No attack indicator of " + position.ToString() + "was found");
                return null;
            }
        }

        /// <summary>
        /// Get a list of indicator on the targeted plane
        /// </summary>
        public List<AttackIndicator> GetIndicator(PlaneType plane)
        {
            List<AttackIndicator> indicators = new List<AttackIndicator>();
            AttackIndicator temp;

            for (int ei = 0; ei < 3; ei++)
            {
                temp = GetIndicator(new WorldPosition(plane, (RunningTrack)ei));
                if (temp != null)
                    indicators.Add(temp);
                else
                {
                    indicators = null;
                    return null;
                }
            }

            return indicators;
        }

        //Override with out list
        public void GetIndicator(PlaneType plane, out List<AttackIndicator> indicators)
        {
            indicators = GetIndicator(plane);
        }

        //Override with out array
        public void GetIndicator(PlaneType plane, out AttackIndicator[] indicators)
        {
            indicators = GetIndicator(plane).ToArray();
        }



        /// <summary>
        /// Get a list of indicator on the targeted track
        /// </summary>
        public List<AttackIndicator> GetIndicator(RunningTrack track)
        {
            List<AttackIndicator> indicators = new List<AttackIndicator>();
            AttackIndicator temp;

            for (int ei = 0; ei < 4; ei++)
            {
                temp = GetIndicator(new WorldPosition((PlaneType)ei, track));

                if (temp != null)
                    indicators.Add(temp);
                else
                {
                    indicators = null;
                    return null;
                }
            }

            return indicators;
        }

        //Override with out list
        public void GetIndicator(RunningTrack track, out List<AttackIndicator> indicators)
        {
            indicators = GetIndicator(track);
        }

        //Override with out array
        public void GetIndicator(RunningTrack track, out AttackIndicator[] indicators)
        {
            indicators = GetIndicator(track).ToArray();
        }
    }
}