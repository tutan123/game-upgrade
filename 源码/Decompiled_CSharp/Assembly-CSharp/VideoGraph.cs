using UnityEngine;
using XNode;

[CreateAssetMenu]
public class VideoGraph : NodeGraph
{
	public string eventName;

	public string xlsxEventKey;

	public void UpdateVideoPath()
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			if (nodes[i] is VideoNode)
			{
				VideoNode videoNode = nodes[i] as VideoNode;
				if (videoNode != null)
				{
					videoNode.videoPath = "";
					videoNode.VideoChange();
					videoNode.sfVideo.unlockVideoPath = "";
					videoNode.sfVideo.VideoChange();
					videoNode.nsVideo.unlockVideoPath = "";
					videoNode.nsVideo.VideoChange();
					videoNode.ptVideo.unlockVideoPath = "";
					videoNode.ptVideo.VideoChange();
				}
			}
			else
			{
				if (!(nodes[i] is IsHasVideo))
				{
					continue;
				}
				IsHasVideo isHasVideo = nodes[i] as IsHasVideo;
				if ((bool)isHasVideo)
				{
					for (int j = 0; j < isHasVideo.videoUnlockDatas.Count; j++)
					{
						isHasVideo.videoUnlockDatas[j].unlockVideoPath = "";
						isHasVideo.videoUnlockDatas[j].VideoChange();
					}
				}
			}
		}
	}
}
