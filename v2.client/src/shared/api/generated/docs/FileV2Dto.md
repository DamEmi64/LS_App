# FileV2Dto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [optional] [default to undefined]
**title** | **string** |  | [optional] [default to undefined]
**description** | **string** |  | [optional] [default to undefined]
**owner** | **string** |  | [optional] [default to undefined]
**_public** | **boolean** |  | [optional] [default to undefined]
**directoryId** | **string** |  | [optional] [default to undefined]
**path** | **string** |  | [optional] [default to undefined]
**fileUsers** | [**Array&lt;FileUserDto&gt;**](FileUserDto.md) |  | [optional] [default to undefined]

## Example

```typescript
import { FileV2Dto } from './api';

const instance: FileV2Dto = {
    id,
    title,
    description,
    owner,
    _public,
    directoryId,
    path,
    fileUsers,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
