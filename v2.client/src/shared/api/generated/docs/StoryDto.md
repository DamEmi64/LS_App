# StoryDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [optional] [default to undefined]
**title** | **string** |  | [default to undefined]
**description** | **string** |  | [default to undefined]
**startDate** | **string** |  | [optional] [default to undefined]
**endDate** | **string** |  | [optional] [default to undefined]
**chapters** | [**Array&lt;ChapterDto&gt;**](ChapterDto.md) |  | [optional] [default to undefined]
**summary** | **string** |  | [optional] [default to undefined]

## Example

```typescript
import { StoryDto } from './api';

const instance: StoryDto = {
    id,
    title,
    description,
    startDate,
    endDate,
    chapters,
    summary,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
