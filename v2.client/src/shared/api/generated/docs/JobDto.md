# JobDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**name** | **string** |  | [default to undefined]
**jobId** | **string** |  | [optional] [default to undefined]
**status** | [**ProgressStatus**](ProgressStatus.md) |  | [optional] [default to undefined]
**requestDate** | **string** |  | [optional] [default to undefined]
**startDate** | **string** |  | [optional] [default to undefined]
**endDate** | **string** |  | [optional] [default to undefined]
**process** | **string** |  | [optional] [default to undefined]
**parent** | **string** |  | [optional] [default to undefined]
**children** | [**Array&lt;JobDto&gt;**](JobDto.md) |  | [optional] [default to undefined]
**jobData** | **string** |  | [optional] [default to undefined]
**operation** | **number** |  | [optional] [default to undefined]

## Example

```typescript
import { JobDto } from './api';

const instance: JobDto = {
    name,
    jobId,
    status,
    requestDate,
    startDate,
    endDate,
    process,
    parent,
    children,
    jobData,
    operation,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
