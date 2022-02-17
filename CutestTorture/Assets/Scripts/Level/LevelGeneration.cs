using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Level
{
    public class LevelGeneration : MonoBehaviour
    {
        public GameObject platformPrefab;
        public GameObject finishPrefab;
        public GameObject coinPrefab;

        public Vector2 minSize;
        public Vector2 maxSize;

        public int platformsMinimalAmount;
        public int platformsMaximalAmount;

        private int _nPlatforms;

        private void Start()
        {
            Assert.IsTrue(platformsMinimalAmount > 0, "Cannot have 0 or less platforms.");
            Assert.IsTrue(platformsMaximalAmount > platformsMinimalAmount, 
                "Maximal amount of platforms can't be less than minimal.");
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            _nPlatforms = Random.Range(platformsMinimalAmount, platformsMaximalAmount);
            var sizes = RandomPlatformSizes();
            var scales = PlatformScales(sizes);
            InstantiatePlatforms(scales);
        }


        [NotNull]
        private IEnumerable<Vector2> RandomPlatformSizes()
        {
            var sizes = new List<Vector2>(_nPlatforms);
            for (var i = 0; i < _nPlatforms; ++i)
            {
                var platformSize = RandomSize();
                sizes.Add(platformSize);
            }

            return sizes;
        }
        
        private Vector2 RandomSize()
        {
            return new Vector2()
            {
                x = Random.Range(minSize.x, maxSize.x),
                y = Random.Range(minSize.y, maxSize.y)
            };
        }
        
        
        [NotNull]
        private List<Vector3> PlatformScales(IEnumerable<Vector2> sizes)
        {
            var scales = new List<Vector3>(_nPlatforms);
            var axis = 1;
            foreach (var size in sizes)
            {
                var length = (int) size.x;
                var width  = (int) size.y;
                scales.Add(PlatformScale(length, width, axis));
                axis = InvertedAxis(axis);
            }

            return scales;
        }
        
        private static Vector3 PlatformScale(int length, int width, int axis)
        {
            return axis == 1 ? new Vector3(width, 1, length) : new Vector3(length, 1, width);
        }
        
        
        private static int InvertedAxis(int axis)
        {
            return ~axis & 1;
        }


        private void InstantiatePlatforms(List<Vector3> scales)
        {
            var previousScale = scales[0];
            var previousPosition = PlatformsStartPosition(scales[0]);
            InstantiatePlatform(previousPosition, previousScale);
            InstantiateCoin(previousPosition);
            scales.RemoveAt(0);
            
            var axis = 0; // x=0, z=1. Z was handled already.
            foreach (var scale in scales)
            {
                var position = PlatformPosition(axis, scale, previousScale, previousPosition);
                InstantiatePlatform(position, scale);
                InstantiateCoin(position);
                axis = InvertedAxis(axis);
                previousPosition = position;
                previousScale = scale;
            }
            
            axis = InvertedAxis(axis); // Because I want to align finish well.
            var finishScale = new Vector3(2, 1, 2);
            var finishPosition = FinishPosition(axis, finishScale, previousScale, previousPosition);
            InstantiateFinish(finishPosition, finishScale);
        }

        private static Vector3 PlatformsStartPosition(Vector3 firstScale)
        {
            return new Vector3(0, 1, firstScale.z / 2);
        }

        private static Vector3 PlatformPosition(int axis, Vector3 scale, Vector3 previousScale, Vector3 previousPosition)
        {
            return axis == 1
                ? ZAlignedPlatformPosition(scale, previousScale, previousPosition)
                : XAlignedPlatformPosition(scale, previousScale, previousPosition);
        }

        private static Vector3 ZAlignedPlatformPosition(Vector3 scale, Vector3 previousScale, Vector3 previousPosition)
        {
            return new Vector3()
            {
                x = previousPosition.x + (previousScale.x / 2) + (scale.x / 2),
                y = 1,
                z = previousPosition.z - (previousScale.z / 2) + (scale.z / 2)
            };
        }
        
        private static Vector3 XAlignedPlatformPosition(Vector3 scale, Vector3 previousScale, Vector3 previousPosition)
        {
            return new Vector3()
            {
                x = previousPosition.x - (previousScale.x / 2) + (scale.x / 2),
                y = 1,
                z = previousPosition.z + (previousScale.z / 2) + (scale.z / 2)
            };
        }
        
        private void InstantiatePlatform(Vector3 position, Vector3 scale)
        {
            var platform = Instantiate(platformPrefab, position, Quaternion.identity);
            platform.transform.localScale = scale;
        }

        private void InstantiateCoin(Vector3 platformCenter)
        {
            Instantiate(coinPrefab, platformCenter + Vector3.up, Quaternion.identity);
        }

        private static Vector3 FinishPosition(int axis, Vector3 scale, Vector3 previousScale, Vector3 previousPosition)
        {
            var position = new Vector3(previousPosition.x, previousPosition.y, previousPosition.z);
            if (axis == 1)
            {
                position.z += (previousScale.z / 2) + (scale.z / 2);
            }
            else
            {
                position.x += (previousScale.x / 2) + (scale.x / 2);
            }

            return position;
        }
        
        private void InstantiateFinish(Vector3 position, Vector3 scale)
        {
            var platform = Instantiate(finishPrefab, position, Quaternion.identity);
            platform.transform.localScale = scale;
        }
    }
}

