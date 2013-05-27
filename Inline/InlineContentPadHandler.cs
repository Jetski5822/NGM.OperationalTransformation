using System.Collections.Generic;
using NGM.OperationalTransformation.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Shapes;
using Orchard.Events;

namespace NGM.OperationalTransformation.Inline {
    public interface IInlineContentHandler : IEventHandler {
        void UpdateEditorShape(IContent content, dynamic context, dynamic shapeFactory);
    }

    public class InlineContentPadHandler : IInlineContentHandler {
        private const string TemplateName = "Parts/Content.Pad";

        public InlineContentPadHandler(IShapeFactory shapeFactory) {
            Shape = shapeFactory;
        }

        public dynamic Shape { get; set; }

        public void UpdateEditorShape(IContent content, dynamic context, dynamic shapeFactory) {
            var part = content.ContentItem.As<ContentPadPart>();

            if (part == null) return;

            var shape = shapeFactory.Parts_ContentPad_InlineEdit(Id: content.Id, ContentItem: content.ContentItem, Prefix: "ContentPadPart");
            ((List<dynamic>)context.ExternalShapes).Add(shape);
        }
    }
}