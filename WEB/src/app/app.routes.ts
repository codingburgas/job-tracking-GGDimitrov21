import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/shared/header/header.component'; // Проверете правилния път до вашия header компонент

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent // Уверете се, че AppHeaderComponent е деклариран тук
  ],
  imports: [
    BrowserModule,
    // ... други модули
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
