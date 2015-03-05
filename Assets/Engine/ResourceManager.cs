using UnityEngine;
using System.Collections.Generic;

namespace TSS {
	public static class ResourceManager {
		private static GUISkin selectBoxSkin;
		public static GUISkin SelectBoxSkin { get { return selectBoxSkin; } }
		public static void StoreSelectBoxItems(GUISkin skin) {
			selectBoxSkin = skin;
		}
	}
}
