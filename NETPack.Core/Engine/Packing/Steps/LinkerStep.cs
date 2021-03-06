﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;

using NETPack.Core.Engine.Packing.Analysis;
using NETPack.Core.Engine.Structs__Enums___Interfaces;
using NETPack.Core.Engine.Utils;
using NETPack.Core.Engine.Utils.Extensions;
using NETPack.Core.Linker.Mono.Linker;
using NETPack.Core.Linker.Mono.Linker.Steps;

namespace NETPack.Core.Engine.Packing.Steps
{
    class LinkerStep<T> : PackingStep
    {
        private AssemblyDefinition _localAsmDef;

        public LinkerStep(AssemblyDefinition asmDef)
            : base(asmDef)
        {
        }

        public override string StepDescription
        {
            get { return ""; }
        }
        public override string StepOutput
        {
            get { return "Linking assembly..."; }
        }

        public override void ProcessStep()
        {
            var pipeLine = GetStandardPipeline();
            var ctx = new LinkContext(pipeLine)
                          {
                              OutputDirectory = Path.GetDirectoryName(Globals.Context.InPath)
                          };

            pipeLine.Process(ctx);
        }

        private static Pipeline GetStandardPipeline()
        {
            var p = new Pipeline();

            p.AppendStep(new ResolveFromAssemblyStep(Globals.Context.InPath));
            p.AppendStep(new LoadReferencesStep());
            p.AppendStep(new BlacklistStep());
            p.AppendStep(new TypeMapStep());
            p.AppendStep(new MarkStep());
            p.AppendStep(new SweepStep());
            p.AppendStep(new CleanStep());
            p.AppendStep(new RegenerateGuidStep());
            p.AppendStep(new OutputStep());

            return p;
        }
    }
}
