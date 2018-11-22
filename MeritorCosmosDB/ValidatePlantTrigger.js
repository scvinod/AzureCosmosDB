function trigger() {
    var createdDocument = getContext().getRequest().getBody();
    var context = getContext();
    var collection = context.getCollection();
    if (createdDocument.DataType == 'Plant') {
        var filterQuery = 'SELECT * FROM root r WHERE r.PlantName = "' + createdDocument.PlantName + '"';
        var accept = collection.queryDocuments(collection.getSelfLink(), filterQuery,
            updateMetadataCallback);
        if (!accept) throw "Unable to update metadata, abort";
    }
    else
        throw "This trigger is only for Plant data type";

    function updateMetadataCallback(err, documents, responseOptions) {
        if (err) throw err;
        if (documents.length > 0) throw 'Plant is already present';

    }
}

