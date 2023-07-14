using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        string PistonGroupName = "StabilizedPistons";
        float PistonRestPos = 1.0f;         //1.0 for SG, 5.0 for LG
        int hzRate = 60;                    //Physics rate
        int multiplier = 1;                 //For when you have pistons stacked in series






        //Do not touch below etc etc





        Vector3 GridVelocity;
        List<IMyPistonBase> pistons;

        public Program()
        {
            //Get groups of pistons
            
            pistons = new List<IMyPistonBase>();

            List<IMyBlockGroup> groups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(groups);

            foreach(IMyBlockGroup group in groups)
            {
                if (group.Name == PistonGroupName)
                {
                    group.GetBlocksOfType(pistons);
                }
               
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {
            GridVelocity = Me.CubeGrid.LinearVelocity;

            //Move pistons
            foreach (IMyPistonBase piston in pistons)
            {
                
                try
                {
                    //Get a target based on the world position and orientation of the piston
                    float target = Vector3.Dot(piston.WorldMatrix.Up, GridVelocity) / hzRate * multiplier + PistonRestPos;
                    piston.MaxLimit = target;
                    piston.MinLimit = target;
                    if (piston.CurrentPosition < target)
                    {
                        piston.Velocity = 5.0f;
                    }
                    else
                    {
                         piston.Velocity = -5.0f;
                    
                    }

                }
                catch (Exception e)
                {
                    pistons.Remove(piston);
                }
            }
        }
    }
}
