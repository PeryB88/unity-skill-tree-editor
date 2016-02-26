﻿using UnityEngine;
using System.Collections.Generic;

namespace Adnc.SkillTreePro {
	[System.Serializable]
	public class SkillTreeEntry {
		public SkillTreeDataBase definition;

		public List<CategoryEntry> categories = new List<CategoryEntry>();
		public Dictionary<string, CategoryEntry> categoriesById = new Dictionary<string, CategoryEntry>();
		public Dictionary<string, CategoryEntry> categoriesByUuid = new Dictionary<string, CategoryEntry>();

		public List<SkillEntry> skills = new List<SkillEntry>();
		public Dictionary<string, SkillEntry> skillsById = new Dictionary<string, SkillEntry>();
		public Dictionary<string, SkillEntry> skillsByUuid = new Dictionary<string, SkillEntry>();

		int _skillPoints;
		public int SkillPoints {
			get {
				return _skillPoints;
			}

			set {
				_skillPoints = Mathf.Max(0, value);
			}
		}

		public SkillTreeEntry (SkillTreeDataBase definition) {
			if (SkillTreeBase.current.debug) Debug.LogFormat("Parsing skill tree: {0}", definition.database.title);

			this.definition = definition;
			definition.database.categories.ForEach(cat => BuildCategory(cat));

			if (SkillTreeBase.current.debug) Debug.LogFormat("Skill tree '{0}' successfully generated", definition.database.title);
		}

		void BuildCategory (SkillCategoryDefinitionBase def) {
			CategoryEntry cat = new CategoryEntry(def, this);
			categories.Add(cat);
			categoriesById[def.id] = cat;
			categoriesByUuid[def.uuid] = cat;
		}

		/// <summary>
		/// Toggle this skill tree on or off
		/// </summary>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		public void ToggleTree (bool enabled) {
			if (enabled) {
				EnableTree();
			} else {
				DisableTree();
			}
		}
			
		/// <summary>
		/// Turns on this skill tree and activates all unlocked skills
		/// </summary>
		public void EnableTree () {
			skills.ForEach(s => s.Skill.Activate(definition));
		}

		/// <summary>
		/// Disables this skill tree and deactivates all active skills
		/// </summary>
		public void DisableTree () {
			skills.ForEach(s => s.Skill.Deactivate(definition));
		}
	}
}