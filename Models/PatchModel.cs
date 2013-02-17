using System;
using System.Linq;
using System.Collections.Generic;
using NGM.ContentPad.Differential.DiffMatchPatch;
using Orchard;

namespace NGM.ContentPad.Models {
    public class PatchModel {
        public List<List<object>> diffs = new List<List<object>>();
        public int start1;
        public int start2;
        public int length1;
        public int length2;
    }

    public class PatchModelToPatchMapper : IMapper<PatchModel, Patch> {
        public Patch Map(PatchModel source) {
            Patch patch = new Patch();
            patch.length1 = source.length1;
            patch.length2 = source.length2;
            patch.start1 = source.start1;
            patch.start2 = source.start2;
            patch.diffs = source.diffs.Select(o => new Diff((Operation)Convert.ToInt32(o.ElementAt(0)), (string)o.ElementAt(1))).ToList();
            return patch;
        }
    }
}