# EventsApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/Events | |
|[**createByIdInvitation**](#createbyidinvitation) | **POST** /api/Events/{id}/invitation | |
|[**createByIdReminder**](#createbyidreminder) | **POST** /api/Events/{id}/reminder | |
|[**deleteById**](#deletebyid) | **DELETE** /api/Events/{id} | |
|[**deleteByIdReminder**](#deletebyidreminder) | **DELETE** /api/Events/{id}/reminder | |
|[**get**](#get) | **GET** /api/Events | |
|[**getById**](#getbyid) | **GET** /api/Events/{id} | |
|[**getMe**](#getme) | **GET** /api/Events/me | |
|[**updateById**](#updatebyid) | **PUT** /api/Events/{id} | |
|[**updateByIdSignIn**](#updatebyidsignin) | **PUT** /api/Events/{id}/signIn | |
|[**updateByIdSignOut**](#updatebyidsignout) | **PUT** /api/Events/{id}/signOut | |

# **create**
> create()


### Example

```typescript
import {
    EventsApi,
    Configuration,
    EventDto
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let eventDto: EventDto; // (optional)

const { status, data } = await apiInstance.create(
    eventDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **eventDto** | **EventDto**|  | |


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

# **createByIdInvitation**
> createByIdInvitation()


### Example

```typescript
import {
    EventsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.createByIdInvitation(
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

# **createByIdReminder**
> createByIdReminder()


### Example

```typescript
import {
    EventsApi,
    Configuration,
    ReminderDto
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let id: string; // (default to undefined)
let reminderDto: ReminderDto; // (optional)

const { status, data } = await apiInstance.createByIdReminder(
    id,
    reminderDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **reminderDto** | **ReminderDto**|  | |
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

# **deleteById**
> deleteById()


### Example

```typescript
import {
    EventsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

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

# **deleteByIdReminder**
> deleteByIdReminder()


### Example

```typescript
import {
    EventsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteByIdReminder(
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
> EventDtoResponseList get()


### Example

```typescript
import {
    EventsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let dateFrom: string; // (optional) (default to undefined)
let dateTo: string; // (optional) (default to undefined)
let category: number; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    page,
    pageSize,
    title,
    dateFrom,
    dateTo,
    category
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **dateFrom** | [**string**] |  | (optional) defaults to undefined|
| **dateTo** | [**string**] |  | (optional) defaults to undefined|
| **category** | [**number**] |  | (optional) defaults to undefined|


### Return type

**EventDtoResponseList**

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
> EventDto getById()


### Example

```typescript
import {
    EventsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

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

**EventDto**

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

# **getMe**
> EventDtoResponseList getMe()


### Example

```typescript
import {
    EventsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let dateFrom: string; // (optional) (default to undefined)
let dateTo: string; // (optional) (default to undefined)
let category: number; // (optional) (default to undefined)

const { status, data } = await apiInstance.getMe(
    page,
    pageSize,
    title,
    dateFrom,
    dateTo,
    category
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **dateFrom** | [**string**] |  | (optional) defaults to undefined|
| **dateTo** | [**string**] |  | (optional) defaults to undefined|
| **category** | [**number**] |  | (optional) defaults to undefined|


### Return type

**EventDtoResponseList**

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
    EventsApi,
    Configuration,
    EventDto
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let id: string; // (default to undefined)
let eventDto: EventDto; // (optional)

const { status, data } = await apiInstance.updateById(
    id,
    eventDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **eventDto** | **EventDto**|  | |
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

# **updateByIdSignIn**
> updateByIdSignIn()


### Example

```typescript
import {
    EventsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdSignIn(
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

# **updateByIdSignOut**
> updateByIdSignOut()


### Example

```typescript
import {
    EventsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdSignOut(
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

