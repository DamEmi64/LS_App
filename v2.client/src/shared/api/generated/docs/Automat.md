# Automat


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**title** | **string** |  | [default to undefined]
**description** | **string** |  | [optional] [default to undefined]
**tasks** | [**Array&lt;Task&gt;**](Task.md) |  | [optional] [default to undefined]
**triggers** | [**Array&lt;Trigger&gt;**](Trigger.md) |  | [optional] [default to undefined]
**lastRun** | **string** |  | [optional] [default to undefined]
**active** | **boolean** |  | [optional] [default to undefined]
**id** | **string** |  | [optional] [default to undefined]
**insDate** | **string** |  | [optional] [default to undefined]
**updDate** | **string** |  | [optional] [default to undefined]
**insBy** | **string** |  | [optional] [default to undefined]
**updBy** | **string** |  | [optional] [default to undefined]

## Example

```typescript
import { Automat } from './api';

const instance: Automat = {
    title,
    description,
    tasks,
    triggers,
    lastRun,
    active,
    id,
    insDate,
    updDate,
    insBy,
    updBy,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
