using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace AHAKuo.Signalia.LocalizationStandalone.Framework.Editors
{
    public static class EditorUtilityMethods
    {
        // Helper function to create a solid color texture
        public static Texture2D RenderTex(int width, int height, Color col)
        {
            int len = Mathf.Max(1, width * height);
            Color[] pix = new Color[len];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;

            var result = new Texture2D(Mathf.Max(1, width), Mathf.Max(1, height));
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        
        /// <summary>
        /// Renders a toolbar with a custom height. Specific for Signalia.
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="options"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static int RenderToolbar(int selected, string[] options, Single height = 24)
        {
            GUI.backgroundColor = Color.gray;
            var result = GUILayout.Toolbar(selected, options, GUILayout.Height(height));
            GUI.backgroundColor = Color.white;
            return result;
        }

        /// <summary>
        /// Renders a Signalia header image with the standard header height.
        /// </summary>
        /// <param name="headerTexture">The header texture to render. Can be null.</param>
        public static void RenderSignaliaHeader(Texture2D headerTexture)
        {
            if (headerTexture != null)
            {
                GUILayout.Label(headerTexture, GUILayout.Height(EditorUtilityConstants.HeaderImageHeight));
            }
        }

        /// <summary>
        /// Renders a Signalia header image with the standard header height.
        /// </summary>
        /// <param name="headerContent">The header GUIContent to render. Can be null.</param>
        public static void RenderSignaliaHeader(GUIContent headerContent)
        {
            if (headerContent != null)
            {
                GUILayout.Label(headerContent, GUILayout.Height(EditorUtilityConstants.HeaderImageHeight));
            }
        }

        /// <summary>
        /// Renders a Signalia header image with optional max width constraint.
        /// </summary>
        /// <param name="headerTexture">The header texture to render. Can be null.</param>
        /// <param name="maxWidth">Optional maximum width constraint.</param>
        public static void RenderSignaliaHeader(Texture2D headerTexture, float maxWidth)
        {
            if (headerTexture != null)
            {
                GUILayout.Label(headerTexture, GUILayout.Height(EditorUtilityConstants.HeaderImageHeight), GUILayout.MaxWidth(maxWidth));
            }
        }

        /// <summary>
        /// Renders a Signalia header image with optional max width constraint.
        /// </summary>
        /// <param name="headerContent">The header GUIContent to render. Can be null.</param>
        /// <param name="maxWidth">Optional maximum width constraint.</param>
        public static void RenderSignaliaHeader(GUIContent headerContent, float maxWidth)
        {
            if (headerContent != null)
            {
                GUILayout.Label(headerContent, GUILayout.Height(EditorUtilityConstants.HeaderImageHeight), GUILayout.MaxWidth(maxWidth));
            }
        }

        [MenuItem("Tools/Signalia Localization/Force Recompile Code")]
        public static void ForceRecompileCode()
        {
            CompilationPipeline.RequestScriptCompilation();
        }
    }

    // ---------------------------
    // Simple search helpers
    // ---------------------------
    internal static class SimpleSearchHelpers
    {
        /// <summary>
        /// Finds the closest match to the search term in the given list.
        /// Returns the index of the best match, or -1 if no good match found.
        /// Excludes "NOAUDIO" from search results.
        /// </summary>
        public static int FindClosestMatch(string searchTerm, IList<string> options, out List<string> multipleMatches)
        {
            multipleMatches = new List<string>();
            
            if (options == null || options.Count == 0)
                return -1;
            
            // If search term is empty, whitespace, or exact match, return all options (except NOAUDIO)
            if (string.IsNullOrWhiteSpace(searchTerm) || 
                options.Any(opt => string.Equals(opt, searchTerm, StringComparison.OrdinalIgnoreCase)))
            {
                for (int i = 0; i < options.Count; i++)
                {
                    string option = options[i] ?? "";
                    if (option != FrameworkConstants.StringConstants.NOAUDIO)
                    {
                        multipleMatches.Add(option);
                    }
                }
                return multipleMatches.Count > 0 ? 0 : -1; // Return first index if we have matches
            }

            string searchLower = searchTerm.ToLowerInvariant();
            var matches = new List<(int index, string option, int score)>();

            for (int i = 0; i < options.Count; i++)
            {
                string option = options[i] ?? "";
                string optionLower = option.ToLowerInvariant();
                
                // Skip NOAUDIO from search results
                if (option == FrameworkConstants.StringConstants.NOAUDIO)
                    continue;
                
                // Exact match gets highest score
                if (optionLower == searchLower)
                {
                    matches.Add((i, option, 1000));
                }
                // Starts with gets high score
                else if (optionLower.StartsWith(searchLower))
                {
                    matches.Add((i, option, 500));
                }
                // Contains gets medium score
                else if (optionLower.Contains(searchLower))
                {
                    matches.Add((i, option, 100));
                }
            }

            if (matches.Count == 0)
                return -1;

            // Sort by score descending
            matches.Sort((a, b) => b.score.CompareTo(a.score));

            // Add all top-scoring matches
            int topScore = matches[0].score;
            foreach (var match in matches)
            {
                if (match.score == topScore)
                {
                    multipleMatches.Add(match.option);
                }
            }

            return matches[0].index;
        }
    }
}
