# AutomationDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [optional] [default to undefined]
**title** | **string** |  | [optional] [default to undefined]
**description** | **string** |  | [optional] [default to undefined]
**tasks** | [**Array&lt;TaskDto&gt;**](TaskDto.md) |  | [optional] [default to undefined]
**triggers** | [**Array&lt;TriggerDto&gt;**](TriggerDto.md) |  | [optional] [default to undefined]
**active** | **boolean** |  | [optional] [default to undefined]

## Example

```typescript
import { AutomationDto } from './api';

const instance: AutomationDto = {
    id,
    title,
    description,
    tasks,
    triggers,
    active,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
