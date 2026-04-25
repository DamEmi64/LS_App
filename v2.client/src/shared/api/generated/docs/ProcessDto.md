# ProcessDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**title** | **string** |  | [default to undefined]
**queue** | **string** |  | [optional] [default to undefined]
**jobs** | [**Array&lt;JobDto&gt;**](JobDto.md) |  | [optional] [default to undefined]
**errors** | [**Array&lt;ProcessErrorDto&gt;**](ProcessErrorDto.md) |  | [optional] [default to undefined]
**startDate** | **string** |  | [optional] [default to undefined]
**endDate** | **string** |  | [optional] [default to undefined]
**percentage** | **number** |  | [optional] [default to undefined]
**status** | [**ProgressStatus**](ProgressStatus.md) |  | [optional] [default to undefined]
**user** | [**UserData**](UserData.md) |  | [optional] [default to undefined]
**id** | **string** |  | [optional] [default to undefined]
**insDate** | **string** |  | [optional] [default to undefined]
**upDate** | **string** |  | [optional] [default to undefined]

## Example

```typescript
import { ProcessDto } from './api';

const instance: ProcessDto = {
    title,
    queue,
    jobs,
    errors,
    startDate,
    endDate,
    percentage,
    status,
    user,
    id,
    insDate,
    upDate,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
