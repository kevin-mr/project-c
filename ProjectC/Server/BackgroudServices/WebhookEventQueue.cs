namespace ProjectC.Server.BackgroudServices
{
    public class WebhookEventQueue
    {
        private readonly Queue<int> webhookEventIds;

        public WebhookEventQueue()
        {
            webhookEventIds = new Queue<int>();
        }

        public void Add(int id)
        {
            webhookEventIds.Enqueue(id);
        }

        public void AddRange(int[] ids)
        {
            foreach (int id in ids)
            {
                webhookEventIds.Enqueue(id);
            }
        }

        public int Read()
        {
            return webhookEventIds.Dequeue();
        }

        public bool IsEmpty()
        {
            return webhookEventIds.Count == 0;
        }
    }
}
