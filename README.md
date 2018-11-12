# Changes from the original version
This module is a fork from [adamconn/sitecore-adaptive-rules](https://github.com/adamconn/sitecore-adaptive-rules). In the original version it is possible to use those rules to personalize content on the site but I have added the possibility to use it in the list manager to create the segmented lists of contacts.

# sitecore-adaptive-rules
This module adds support for adaptive rules for the Sitecore Rules Engine. Adaptive rules are useful when properties on conditions and actions have dependencies on one another. 

You can [watch a video](http://youtu.be/M2m2vJ5diEI) that shows what this module does.

### Module download
The adaptive rules installation package is available [here](https://github.com/vgbenjamin/sitecore-adaptive-rules/tree/master/sitecore/).

A package that includes sample conditions that demonstrate how to use adaptive rules is available [here](https://github.com/vgbenjamin/sitecore-adaptive-rules/tree/master/sitecore/).

### Sample use case
For example, imagine a case where an author wants to personalize a page based a video the visitor watched the last time he visited the site. If the visitor watched a video about warm-weather vacation destinations the author wants the visitor to see a promotion about travel to tropical islands. If the visitor watched videos about skiing and snowboarding the author wants the visitor to see a promotion about travel with ski resorts.

With adaptive rules, when the author would select a condition with 2 parameters. The 1st parameter is where the author selects "warm-weather vacation destinations". Based on that selection, the 2nd parameter is limited to promotions that are related to warm-weather vacation destinations.

### How Adaptive Rules work
Adaptive macros are the key to how adaptive rules work. Macros are the components in the Sitecore Rules Engine that present the user interface used to configure rules. Adaptive macros dynamically determine the appropriate user interface to display.

### Adaptive Macros
This module includes 3 macros.

#### Adaptive Tree
The adaptive tree displays the children of an item specified in another parameter on a condition or action.

The following is an example of a condition that is configured to use the adaptive tree macro:
```
where product type is [productType,Tree,root={495B562E-5DF9-458F-B646-329DCA2BF5E0},specific type] 
and promotion is [promotionId,AdaptiveTree,dependency=productType,specific promotion] 
```

#### Adaptive Operator
One of the most common uses of the Sitecore Rules Engine is to configure the comparison of one value to another. The adaptive operator displays operators that are appropriate for the values that are being compared.

For example, if 2 string values are being compared the macro will display the string operators. If 2 numbers are being compared the macro will display the number operators.

The adaptive operator macro can also use custom operators to handle specific requirements. 

The following is an example of a condition that is configured to use the adaptive operator macro:
```
where the contact facet [contactFacetId,Tree,root={C630AE5C-9EA7-4F22-9EBA-2B385425F78B},facet name] 
has a member [contactFacetMemberId,AdaptiveTree,dependency=ContactFacetId,member name] 
with a value [operator,AdaptiveOperator,dependency=ContactFacetMemberId,operator] 
[value,AdaptiveValue,dependency=ContactFacetMemberId,value]
```

#### Adaptive Value
The adaptive value macro is similar to the adaptive operator. When 2 numbers are compared it is important that the Sitecore user be prompted to enter numbers. 

This macro ensures that the Sitecore user is presented with an appropriate user interface for specifying the value when the user configures a comparison.

The following is an example of a condition that is configured to use the adaptive value macro:
```
where the contact facet [contactFacetId,Tree,root={C630AE5C-9EA7-4F22-9EBA-2B385425F78B},facet name] 
has a member [contactFacetMemberId,AdaptiveTree,dependency=ContactFacetId,member name] 
with a value [operator,AdaptiveOperator,dependency=ContactFacetMemberId,operator] 
[value,AdaptiveValue,dependency=ContactFacetMemberId,value]
```
