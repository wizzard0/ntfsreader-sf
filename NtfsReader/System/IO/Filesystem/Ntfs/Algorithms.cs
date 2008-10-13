using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO.Filesystem.Ntfs
{
    public static class Algorithms
    {
        public static IDictionary<UInt32, List<INode>> AggregateByFragments(IEnumerable<INode> nodes, UInt32 minimumFragments)
        {
            Dictionary<UInt32, List<INode>> fragmentsAggregate = new Dictionary<UInt32, List<INode>>();

            foreach (INode node in nodes)
            {
                IList<IStream> streams = node.Streams;
                if (streams == null || streams.Count == 0)
                    continue;

                IList<IFragment> fragments = streams[0].Fragments;
                if (fragments == null)
                    continue;

                UInt32 fragmentCount = (UInt32)fragments.Count;

                if (fragmentCount < minimumFragments)
                    continue;

                List<INode> nodeList;
                fragmentsAggregate.TryGetValue(fragmentCount, out nodeList);

                if (nodeList == null)
                {
                    nodeList = new List<INode>();
                    fragmentsAggregate[fragmentCount] = nodeList;
                }

                nodeList.Add(node);
            }

            return fragmentsAggregate;
        }

        public static IDictionary<UInt64, List<INode>> AggregateBySize(IEnumerable<INode> nodes, UInt64 minimumSize)
        {
            Dictionary<UInt64, List<INode>> sizeAggregate = new Dictionary<ulong, List<INode>>();

            foreach (INode node in nodes)
            {
                if ((node.Attributes & Attributes.Directory) != 0 || node.Size < minimumSize)
                    continue;

                List<INode> nodeList;
                sizeAggregate.TryGetValue(node.Size, out nodeList);

                if (nodeList == null)
                {
                    nodeList = new List<INode>();
                    sizeAggregate[node.Size] = nodeList;
                }

                nodeList.Add(node);
            }

            return sizeAggregate;
        }
    }
}
