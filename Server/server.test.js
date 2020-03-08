const Server = require('./server')
const WebSocket = require('ws');

let client

describe('server', () => {
  //new Server(3000);
  
  beforeEach(()=>{
    client = new WebSocket("ws://localhost:3000")
  })

  afterEach(()=>{
    client.close()
  })

  it('connection', done=>{
    client.on('message', data=>{
      let d = JSON.parse(data)
      expect(d.Type).toBe('join')
      done()
    })
  })

  it('push message', done=>{
    client2 = new WebSocket("ws://localhost:3000")
    client2.on('open', ()=>{
      // client2が接続完了後にclientでメッセージを送る
      client.send(JSON.stringify({ Type: 'broadcast', Data: 'hello' }))
    })
    client2.on('message', data=>{
      let d = JSON.parse(data)
      if(d.Type === 'broadcast'){
        client2.close()
        expect(d.Data).toBe('hello')
        done()
      }
    })
  })
})
