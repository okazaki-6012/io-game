const net = require('net')

class Server {
  constructor(port){
    this.clients = []
    this.server = net.createServer(this.connectionListener.bind(this)).listen(port)    
    console.log(`server running at port ${port}\n`)
  }

  /** Initializing the client when connecting */
  connectionListener(socket){
    // 接続があった際に掃除する
    this.clients = this.clients.filter(c=>!c.destroyed)
    
    socket.name = socket.remoteAddress+":"+`${Math.random()}`.slice(2,14)
    this.clients.push(socket)
    p(`Join ${socket.name}`)
    
    socket.write('welcome!!')
    
    socket.on('data', data => {
      this.broadcast(data, socket)
    })

    socket.on('end', () => {
      this.clients.slice(this.clients.indexOf(socket), 1)
      p(`Exit ${socket.name}`)
    })
  }

  /** Push to other clients */
  broadcast(message, sender){
    for(let c of this.clients){
      if(c !== sender && !c.destroyed){
        c.write(message)
      }
    }
    p(message)
  }

  /** close server */
  close(){
    this.server.close()
  }
}

function p(message){
  process.stdout.write(message+'\n')
}

module.exports = Server
