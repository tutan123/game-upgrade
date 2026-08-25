using System.Collections.Generic;
using FrameWork;
using XNode;

[CreateNodeMenu("Video/ShowIf")]
public class ShowIf : Node
{
	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public string input;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public string output;

	public List<PropertyData> propertyTypes = new List<PropertyData>();

	protected override void Init()
	{
		base.Init();
	}

	public override object GetValue(NodePort port)
	{
		return null;
	}

	public bool IsSuc()
	{
		return propertyTypes.IsSuc();
	}
}
