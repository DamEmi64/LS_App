# AutomationsApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/Automations | |
|[**deleteById**](#deletebyid) | **DELETE** /api/Automations/{id} | |
|[**get**](#get) | **GET** /api/Automations | |
|[**getById**](#getbyid) | **GET** /api/Automations/{id} | |
|[**getByIdTasks**](#getbyidtasks) | **GET** /api/Automations/{id}/tasks | |
|[**updateById**](#updatebyid) | **PUT** /api/Automations/{id} | |
|[**updateByIdTurnoff**](#updatebyidturnoff) | **PUT** /api/Automations/{id}/turnoff | |
|[**updateByIdTurnon**](#updatebyidturnon) | **PUT** /api/Automations/{id}/turnon | |

# **create**
> create()


### Example

```typescript
import {
    AutomationsApi,
    Configuration,
    AutomationDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let automationDto: AutomationDto; // (optional)

const { status, data } = await apiInstance.create(
    automationDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **automationDto** | **AutomationDto**|  | |


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
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

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
> AutomatResponseList get()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    order,
    page,
    pageSize,
    orderBy,
    title
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **order** | [**string**] |  | defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **orderBy** | [**string**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|


### Return type

**AutomatResponseList**

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
> Automat getById()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

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

**Automat**

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

# **getByIdTasks**
> TaskResponseList getByIdTasks()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getByIdTasks(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**TaskResponseList**

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
    AutomationsApi,
    Configuration,
    AutomationDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)
let automationDto: AutomationDto; // (optional)

const { status, data } = await apiInstance.updateById(
    id,
    automationDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **automationDto** | **AutomationDto**|  | |
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

# **updateByIdTurnoff**
> updateByIdTurnoff()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdTurnoff(
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

# **updateByIdTurnon**
> updateByIdTurnon()


### Example

```typescript
import {
    AutomationsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new AutomationsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdTurnon(
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

