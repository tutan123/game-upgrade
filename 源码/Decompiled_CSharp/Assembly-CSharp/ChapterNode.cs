using System.Collections.Generic;
using XNode;
using Xlsx;

[CreateNodeMenu("Video/ChapterNode")]
public class ChapterNode : Node
{
	public string chapterName;

	public List<PropertyData> propertyType;

	public List<PropertyData> showPropertyType;

	public bool isUseTips;

	public Xlsx_Language_Key xlsxLanguageKey;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public ChapterNode chapterNode;

	protected override void Init()
	{
		base.Init();
	}

	public override object GetValue(NodePort port)
	{
		if (port.fieldName == "chapterNode")
		{
			return this;
		}
		return null;
	}
}
