using System.Collections.Generic;
using XNode;
using Xlsx;

namespace FrameWork.AchievementTool;

[NodeWidth(300)]
[CreateNodeMenu("Achievement/AchievementNode")]
public class AchievementNode : Node
{
	public List<PropertyData> propertyData;

	public List<Xlsx_Achievement_Key> achievements;
}
