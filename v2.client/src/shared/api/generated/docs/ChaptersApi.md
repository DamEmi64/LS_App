# ChaptersApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/Chapters | |
|[**deleteById**](#deletebyid) | **DELETE** /api/Chapters/{id} | |
|[**get**](#get) | **GET** /api/Chapters | |
|[**getById**](#getbyid) | **GET** /api/Chapters/{id} | |
|[**updateById**](#updatebyid) | **PUT** /api/Chapters/{id} | |
|[**updateByIdEnd**](#updatebyidend) | **PUT** /api/Chapters/{id}/end | |
|[**updateByIdFlow**](#updatebyidflow) | **PUT** /api/Chapters/{id}/flow | |
|[**updateByIdPublish**](#updatebyidpublish) | **PUT** /api/Chapters/{id}/publish | |
|[**updateByIdStart**](#updatebyidstart) | **PUT** /api/Chapters/{id}/start | |

# **create**
> create()


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

const { status, data } = await apiInstance.create(
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

# **deleteById**
> deleteById()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteById(
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

# **get**
> ChapterResponseList get()


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

const { status, data } = await apiInstance.get(
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

# **getById**
> Chapter getById()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getById(
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

# **updateById**
> updateById()


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

const { status, data } = await apiInstance.updateById(
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

# **updateByIdEnd**
> updateByIdEnd()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdEnd(
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

# **updateByIdFlow**
> updateByIdFlow()


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

const { status, data } = await apiInstance.updateByIdFlow(
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

# **updateByIdPublish**
> updateByIdPublish()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdPublish(
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

# **updateByIdStart**
> updateByIdStart()


### Example

```typescript
import {
    ChaptersApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new ChaptersApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdStart(
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

