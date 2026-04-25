# ChaptersApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createChapter**](#createchapter) | **POST** /Chapters | |
|[**deleteChapterById**](#deletechapterbyid) | **DELETE** /Chapters/{id} | |
|[**getChapter**](#getchapter) | **GET** /Chapters | |
|[**getChapterById**](#getchapterbyid) | **GET** /Chapters/{id} | |
|[**updateChapterById**](#updatechapterbyid) | **PUT** /Chapters/{id} | |
|[**updateChapterByIdEnd**](#updatechapterbyidend) | **PUT** /Chapters/{id}/end | |
|[**updateChapterByIdFlow**](#updatechapterbyidflow) | **PUT** /Chapters/{id}/flow | |
|[**updateChapterByIdPublish**](#updatechapterbyidpublish) | **PUT** /Chapters/{id}/publish | |
|[**updateChapterByIdStart**](#updatechapterbyidstart) | **PUT** /Chapters/{id}/start | |

# **createChapter**
> createChapter()


### Example

```typescript
import {
    ChaptersApi,
    Configuration,
    ChapterDto
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let body: ChapterDto; // (optional)

const { status, data } = await apiInstance.createChapter(
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **ChapterDto**|  | |


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deleteChapterById**
> deleteChapterById()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteChapterById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getChapter**
> ChapterResponseList getChapter()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let startFrom: string; // (optional) (default to undefined)
let startTo: string; // (optional) (default to undefined)
let endFrom: string; // (optional) (default to undefined)
let endTo: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getChapter(
    page,
    pageSize,
    title,
    startFrom,
    startTo,
    endFrom,
    endTo
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **startFrom** | [**string**] |  | (optional) defaults to undefined|
| **startTo** | [**string**] |  | (optional) defaults to undefined|
| **endFrom** | [**string**] |  | (optional) defaults to undefined|
| **endTo** | [**string**] |  | (optional) defaults to undefined|


### Return type

**ChapterResponseList**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getChapterById**
> Chapter getChapterById()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getChapterById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Chapter**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateChapterById**
> updateChapterById()


### Example

```typescript
import {
    ChaptersApi,
    Configuration,
    ChapterDto
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)
let body: ChapterDto; // (optional)

const { status, data } = await apiInstance.updateChapterById(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **ChapterDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateChapterByIdEnd**
> updateChapterByIdEnd()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateChapterByIdEnd(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateChapterByIdFlow**
> updateChapterByIdFlow()


### Example

```typescript
import {
    ChaptersApi,
    Configuration,
    FlowDto
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)
let body: FlowDto; // (optional)

const { status, data } = await apiInstance.updateChapterByIdFlow(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **FlowDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateChapterByIdPublish**
> updateChapterByIdPublish()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateChapterByIdPublish(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateChapterByIdStart**
> updateChapterByIdStart()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateChapterByIdStart(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

