using System;
using System.Collections.Generic;
using System.Linq;
using NGM.OperationalTransformation.Differential.DiffMatchPatch;
using NGM.OperationalTransformation.Models;
using NUnit.Framework;
using Newtonsoft.Json;

namespace NGM.ContentPadTests.Differential
{
    [TestFixture]
    public class DiffMatchPatchJavascriptToCSharpPatchTests {
        private diff_match_patch _diffMatchPatch;

        [SetUp]
        public void SetUp() {
            _diffMatchPatch = new diff_match_patch();
        }

        [Test]
        public void ShouldApplyPatch() {
            var json =
                "[{\"diffs\":[[0,\"Welcome to Orchard!\"],[1,\"kk\"],[0,\"Welcome to Orchard!\"]],\"start1\":0,\"start2\":0,\"length1\":38,\"length2\":40}]";
            var patches = JsonConvert.DeserializeObject<List<PatchModel>>(json);


            var patchModelToPatchMapper = new PatchModelToPatchMapper();
            var patchesTranslated = patches.Select(o => patchModelToPatchMapper.Map(o)).ToList();

            
            var sample = _diffMatchPatch.patch_make("Welcome to Orchard!", "Welcome to Orchard!l");
            Object[] results = _diffMatchPatch.patch_apply(sample, "Welcome to Orchard!");

            var patchesApplied = _diffMatchPatch.patch_apply(patchesTranslated, "Welcome to Orchard!");

            //Assert.That(patchesApplied);

        }

    }
}
