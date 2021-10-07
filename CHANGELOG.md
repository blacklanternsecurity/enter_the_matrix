CHANGELOG
============
# October 7th, 2021

## Changes

- Threat Trees
	+ Now possible to edit categories in an existing threat tree
- Threat Narrative Graphs
	+ Updated to use D3.js library for graphs
- Template Packs
	+ Can import/export template packs as JSON files
- Multiple bug fixes in all areas

# June 8th, 2021

## MITRE ATT&CK v9

Techniques have been edited to reflect changes present in ATT&CK v9

## New Features:

- Threat Trees
	+ MITRE ATT&CK IDs are pulled from an entire assessment and populate a new design page for threat trees. From the design page you can customize many aspects of how the graph will appear when exported. This can be found by navigating to an Assessment and clicking the THREAT TREE button. Please note that once categories have been created for a threat tree, those are not changeable without deleting the tree and starting over. This was implemented to aid when creating the outbrief graphs we've seen lately
- MITRE ATT&CK For ICS
  + Now included in your options when selecting an appropriate ATT&CK technique under the EditEvent and EditSteplate pages
- HTML Reports
  + If for whatever reason you need a tabular version of a threat matrix, select the EXPORT button while in a Scenario page and select HTML. This will present you with a tabled view of the entire scenario. Printing from this page will print in landscape mode (in Chrome automatically, in FF you'll need to click Landscape when printing) in black and white
- Living XLSX Report
  + Exported XLSX Threat Matrices will now auto-update "Overall Likelihood" to it's appropriate value when "Likelihood of Initiation" and "Likelihood of Adverse Impact" are changed. "Overall Risk" is also automatically updated as "Overall Likelihood" and "Impact" values change
- Readable Scenario Graph Edge Labels
  + The numbering that accompanies scenario graph edges is now styled to be more visible for those that found them hard to read
- BLS Descriptions
  + While editing an event in a scenario, clicking the info helper button will now present you with the NIST descriptions as well as BLS descriptions. Hopefully these will make it easier to interpret and apply values to the various factors. Feedback is welcome if there is a great disagreement on interpretation
  
## Bug Fixes:

- When creating the labels for each node in a scenario graph, it is no longer necessary to escape quotations to avoid breaking the graph while rendering.

# February 25th, 2021

## MITRE ATT&CK v8.2

FactorModels->Techniques has been updated to incorporate changes made to the ATT&CK framework in version 8.2.

# December 1st, 2020

## Schema Change

This update includes changes to the application schema. Because of this, if you had used the previous version you may need to update your data to continue proper functionality

- The documents of concern are in the Steps collection
- The specific entity involved is the GraphNode and more specifically the GraphNode.ParentId
  - ParentId was previously a string value, moving forward it is a string[] value
- At the end of this changelog, we have included the script we used to migrate our data in hopes it will help others migrate theirs

## New Features

Below are a list of the features implemented in this release

- Individual events in a scenario may now have graph nodes with multiple parent nodes. This allows for more detailed graph generation showing relations between nodes that lead to a given outcome
- Multiple areas are now filterable based on user input
  - MITRE ATT&CK selection
  - Importing Steplates into Events
  - Editing Steplates
- Graph Nodes have a preview icon displayed once selected while editing a Step
- Steps that have been created under a Scenario can be exported to the Steplates section once saved for later use
- MITRE ATT&CK
  - MITRE has since release of ETM updated the ATT&CK framework
  - This change deprecated the PRE-ATT&CK tactics and techniques and rolled them into ENTERPRISE-ATT&CK
  - As of the time of this update, ETM is in line with current MITRE ATT&CK techniques that have also been added
  - Previous threat matrices generated with ETM will still provide use to client's as MITRE provides redirects to the deprecated techniques

## Data Migration Steps

### First where GraphNode is null, attempt to set it with it's members:
```
> db.Steps.find({"GraphNode":null}).forEach(function(myDoc){ db.Steps.update( { _id: myDoc._id }, { "$set": { "GraphNode.ParentId": "na", "GraphNode.EntityType": "na", "GraphNode.EntityDescription": "na", "GraphNode.Risk": "na", "GraphNode._id": "na" } } ); })
```

### Second, there may be some nulls there still. Check
```
> db.Steps.find({"GraphNode":null}).count()
```

### Third, if there are, unset GraphNode
```
> db.Steps.find({"GraphNode":null}).forEach(function(myDoc){ db.Steps.update( { _id: myDoc._id }, { "$unset" : { "GraphNode": null } } ); })
```

### Fourth, re-set for those two
```
> db.Steps.find({"GraphNode":null}).forEach(function(myDoc) { db.Steps.update( { _id: myDoc._id }, { "$set": { "GraphNode.ParentId": "na", "GraphNode.EntityType": "na", "GraphNode.EntityDescription": "na", "GraphNode.Risk": "na", "GraphNode._id": "na" } } ); })
```

### Fifth, check and replace any ParentIds that are null
```
> db.Steps.find({"GraphNode.ParentId":null}).forEach(function(myDoc){ db.Steps.update( { _id: myDoc._id }, { "$set": { "GraphNode.ParentId": "na" } } ); })
```

### Sixth, turn all of your string type ParentIds to array type
```
> db.Steps.find().forEach(function(myDoc){ db.Steps.update( { _id: myDoc._id }, { "$set": { "GraphNode.ParentId": [myDoc.GraphNode.ParentId] } } ); })
```

### Finally, confirm the array count is the total count of entries
```
> db.Steps.find({"GraphNode.ParentId": {"$type": "array"}}).count()
> db.Steps.find().count()
```
