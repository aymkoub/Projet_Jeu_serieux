using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terriflux.Programs;

namespace Terriflux.Programs;
/// <summary>
/// Abstract node
/// </summary>
public partial class Building : Cell
{
    protected readonly double[] impacts;
    protected readonly Dictionary<FlowKind, int> needs;
    protected readonly Dictionary<FlowKind, int> minimalProduction;

    // children
    protected Color colorOfDot; 
    protected Sprite2D _buildingSprite;
    protected Polygon2D _dot;

    public Building(double[] impacts, Dictionary<FlowKind, int> needs, Dictionary<FlowKind, int> minimalProduction, Color colorOfDot) : base()
    {
        if (impacts.Length != 3) throw new ArgumentException("impacts.Lengt must be 3", nameof(impacts));

        this.impacts = impacts;
        this.needs = needs;
        this.minimalProduction = minimalProduction;
        this.colorOfDot = colorOfDot;
    }

    public override void _Ready()
    {
        base._Ready();
        _buildingSprite = GetNode<Sprite2D>("BuildingSprite");
        this._buildingSprite.Texture = GetIcon();
        _dot = GetNode<Polygon2D>("Dot");
        this._dot.Color = colorOfDot;
    }

    /// <returns>Returns the image representing the building.</returns>
    public Texture2D GetIcon()
    {
        return GD.Load<Texture2D>(PATH_IMAGES + GetType().Name.ToLower() + ".png");
    }

    /// <returns>Social, Economy, Ecology</returns>
    public double[] GetImpacts()
    {
        return impacts.ToArray();
    }

    public FlowKind[] GetNeeds()
    {
        return this.needs.Keys.ToArray();
    }
    
    public FlowKind[] GetProduction()
    {
        return this.minimalProduction.Keys.ToArray();
    }

    public int GetNeedOf(FlowKind flow)
    {
        return this.needs[flow];
    }
    
    /// <param name="flow"></param>
    /// <returns>Minimal production of </returns>
    public int GetProductOf(FlowKind flow)
    {
        return this.minimalProduction[flow];
    }

    public string GetHelp()
    {
        StringBuilder sb = new();
        // impacts
        sb.Append("Impacts: ");
        foreach (double impact in impacts)
        {
            sb.Append($"{impact} ");
        }
        sb.AppendLine();
        // needs
        if (needs.Count > 0)
        {
            sb.Append("Needs: ");
            foreach (KeyValuePair<FlowKind, int> kvp in needs)
            {
                sb.Append(kvp.Key.ToString() + " x " + kvp.Value.ToString() + " ");
            }
        }
        else sb.Append("No needs");
        sb.AppendLine();
        // product
        if (minimalProduction.Count > 0)
        {
            sb.Append("Products (minimal): ");
            foreach (KeyValuePair<FlowKind, int> kvp in minimalProduction)
            {
                sb.Append(kvp.Key.ToString() + " x " + kvp.Value.ToString() + " ");
            }
        }
        else sb.Append("No production");
        sb.AppendLine();
        return sb.ToString();
    }

    public override string Verbose()
    {
        return base.Verbose() + "\n" + GetHelp();
    }
}