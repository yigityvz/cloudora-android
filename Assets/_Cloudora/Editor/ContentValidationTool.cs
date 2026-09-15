using Cloudora.Level;
using UnityEditor;
using UnityEngine;

namespace Cloudora.Editor
{
    public static class ContentValidationTool
    {
        [MenuItem("Cloudora/Validate 10,000 Levels")]
        public static void RunBatch()
        {
            int failures = 0;
            float minimum = float.MaxValue;
            float maximum = float.MinValue;
            for (int level = 16; level < 10016; level++)
            {
                try
                {
                    LevelDefinition definition = LevelGenerator.Generate(level);
                    if (!PuzzleValidator.Validate(definition, out string error))
                    {
                        failures++;
                        Debug.LogError($"Validation level={level} seed={definition.seed}: {error}");
                    }
                    minimum = Mathf.Min(minimum, definition.difficultyScore);
                    maximum = Mathf.Max(maximum, definition.difficultyScore);
                }
                catch (System.Exception exception)
                {
                    failures++;
                    Debug.LogError($"Generation level={level}: {exception.Message}");
                }
            }
            Debug.Log($"CLOUDORA_VALIDATION total=10000 failures={failures} difficulty_min={minimum:F1} difficulty_max={maximum:F1}");
            if (Application.isBatchMode) EditorApplication.Exit(failures == 0 ? 0 : 1);
        }
    }
}
