using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Messaging
{
    public  class MessageResultCollection
    {
        private List<MessageResult> _messages = new();

        public MessageResultCollection() { }


        public string[] GetDataForLogFile()
        {
            List<string> messages = new List<string>();
            foreach (MessageResult result in _messages)
            {
                string message = $"{result.EntryDateTime.ToString()} - {result.MessageResultType}: {result.Message}";
                messages.Add(message);
            }
            return messages.ToArray();
        }

        /// <summary>
        /// Returns all messages in the collection.
        /// </summary>
        /// <returns></returns>
        public List<MessageResult> GetMessages()
        {
            return _messages;
        }

        /// <summary>
        /// Adds a message to the collection.
        /// </summary>
        /// <param name="message"></param>
        public void Add(MessageResult message)
        {
            if (message != null)
            {
                _messages.Add(message);
            }
        }
    }
}
