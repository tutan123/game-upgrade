using System.Collections;

namespace RenderHeads.Media.AVProVideo;

public interface IVariants : IEnumerable
{
	int Count { get; }

	Variant Current { get; }

	Variant this[int index] { get; }

	Variant GetSelectedVariant();

	void SelectVariant(Variant variant);
}
