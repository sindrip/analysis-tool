namespace Analysis.CFG
{
    public struct FlowEdge
    {
        public int Source { get; }
        public int Dest { get; }

        public FlowEdge(int source, int dest)
        {
            Source = source;
            Dest = dest;
        }
    }
}