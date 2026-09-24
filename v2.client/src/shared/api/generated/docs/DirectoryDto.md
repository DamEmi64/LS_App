# DirectoryDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [optional] [default to undefined]
**title** | **string** |  | [optional] [default to undefined]
**parentId** | **string** |  | [optional] [default to undefined]
**childDirectoryCount** | **number** |  | [optional] [default to undefined]
**fileCount** | **number** |  | [optional] [default to undefined]
**owner** | **string** |  | [optional] [default to undefined]
**_public** | **boolean** |  | [optional] [default to undefined]
**fileUsers** | [**Array&lt;FileUserDto&gt;**](FileUserDto.md) |  | [optional] [default to undefined]

## Example

```typescript
import { DirectoryDto } from './api';

const instance: DirectoryDto = {
    id,
    title,
    parentId,
    childDirectoryCount,
    fileCount,
    owner,
    _public,
    fileUsers,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
